# Dokumentacja testów backendu

Dokument opisuje testy w projekcie `EventMarketplace.Tests`. Testy sprawdzają obsługę rejestracji i logowania użytkownika oraz edycji, zgłaszania i usuwania wydarzeń.

## Jak uruchomić

W katalogu backendu:

```bash
dotnet test EventMarketplace.Tests/EventMarketplace.Tests.csproj
```

Testy jednostkowe korzystają z atrap zależności (`Moq`), więc nie łączą się z prawdziwą bazą ani Azure Blob Storage. Testy integracyjne używają bazy EF Core InMemory tworzonej osobno dla każdego testu; usuwanie obrazu w Azure jest w nich symulowane.

## Rejestracja użytkownika

### Testy jednostkowe — `RegisterUserCommandHandlerTests`

| Przypadek | Dane wejściowe / przygotowanie | Oczekiwany rezultat |
|---|---|---|
| Poprawna rejestracja | `new.user@example.com`, hasło i potwierdzenie `Password.123`, konto uczestnika; rola Participant istnieje | Powstaje użytkownik z podanym e-mailem, hasło jest haszowane, przypisana jest rola Participant i zapis jest wywołany raz. |
| Zajęty e-mail | Jak wyżej, ale repozytorium zgłasza, że e-mail jest używany | Rzucony `EmException`; użytkownik nie jest dodawany ani zapisywany. |
| Różne hasła | Hasło `Password.123`, potwierdzenie `Different.123` | Rzucony `EmException`; hasło nie jest haszowane, użytkownik nie jest dodawany ani zapisywany. |
| Brak roli uczestnika | Poprawne dane, ale repozytorium nie znajduje roli Participant | Rzucony `EmNotFoundException`; brak dodawania i zapisu. |

### Test integracyjny — `RegisterUserCommandHandlerIntegrationTests`

**Dane:** rola Participant w bazie InMemory oraz `integration.user@example.com`, hasło i potwierdzenie `Password.123!`.

**Oczekiwany rezultat:** po obsłużeniu komendy użytkownik istnieje w bazie, zapisane hasło różni się od jawnego, ale przechodzi weryfikację `ValidPassword`; użytkownik ma przypisaną rolę Participant.

**Jak działa:** test składa prawdziwy handler, `TestUnitOfWork`, kontekst EF Core InMemory i `PasswordManager`. Po rejestracji ponownie odczytuje użytkownika wraz z rolami z kontekstu. Sprawdza więc współpracę tych elementów i zapis danych, bez zewnętrznej bazy.

## Logowanie użytkownika — `LoginUserCommandHandlerTests`

W każdym przypadku e-mail wejściowy to `new.user@example.com`.

| Przypadek | Dane wejściowe / przygotowanie | Oczekiwany rezultat |
|---|---|---|
| Nieznany e-mail | Repozytorium zwraca brak użytkownika | `EmNotFoundException`; hasło nie jest sprawdzane i zapis nie jest wykonywany. |
| Błędne hasło | Użytkownik istnieje z hashem `stored-hash`, podane hasło `Wrong.123`, weryfikacja zwraca `false` | `EmException`; tokeny nie są generowane ani zapisywane. |
| Poprawne dane | Użytkownik istnieje, `Password.123` przechodzi weryfikację; generator zwraca access token `access-token` i refresh token `refresh-token-value` | Wynik zawiera access token; refresh token jest dołączony do odpowiedzi, zapisany dla użytkownika jako nieodwołany z datą wygaśnięcia, a zapis zmian wywołany raz. |

**Jak działa:** zależności handlera (repozytoria, weryfikator hasła i generator JWT) są atrapami. Test ustala odpowiedź każdej z nich i sprawdza zarówno wynik, jak i to, które operacje handler wykonał. Nie sprawdza kryptograficznej poprawności JWT.

## Obsługa wydarzeń

### Zgłaszanie wydarzenia — `SubmitEventCommandHandlerTests`

| Przypadek | Dane wejściowe / przygotowanie | Oczekiwany rezultat |
|---|---|---|
| Wydarzenie w wersji roboczej | Istniejące wydarzenie o statusie `Draft` i jego identyfikator | Status zmienia się na `Submitted`, ustawione zostaje `UpdatedAt`. |
| Już zgłoszone wydarzenie | Istniejące wydarzenie o statusie `Submitted` | `EmConflictException`; status pozostaje `Submitted`. |
| Brak wydarzenia | Identyfikator, którego repozytorium nie znajduje | `EmNotFoundException`. |

**Jak działa:** repozytorium jest atrapą zwracającą przygotowane wydarzenie albo brak wyniku. Test wywołuje handler komendą z identyfikatorem i sprawdza zmianę stanu lub wyjątek.

### Edycja wydarzenia — `EditEventServiceTests`

| Przypadek | Dane wejściowe / przygotowanie | Oczekiwany rezultat |
|---|---|---|
| Edycja istniejącego wydarzenia | Istniejące wydarzenie; żądanie z tym samym ID, tytułem `Updated title` i nowymi datami rozpoczęcia oraz zakończenia | Mapowanie aktualizuje tytuł i daty, a `SaveChangesAsync` jest wywołane raz. |
| Nieznany identyfikator | Żądanie zawiera ID nieobecne w repozytorium | `EmNotFoundException`; mapowanie i zapis nie są wywołane. |

**Jak działa:** test prawidłowej edycji ustawia atrapę mappera tak, by zmieniła tytuł obiektu. Serwis sam aktualizuje zakres dat. Test weryfikuje końcowy obiekt i wywołanie zapisu; nie testuje konfiguracji AutoMappera.

### Usuwanie wydarzenia — `DeleteEventServiceTests`

| Przypadek | Dane wejściowe / przygotowanie | Oczekiwany rezultat |
|---|---|---|
| Usunięcie wersji roboczej | Istniejące wydarzenie `Draft` z adresem obrazu kończącym się na `poster.jpg` | `IsDeleted` ma wartość `true`, obraz jest usuwany z kontenera `events`, transakcja jest zatwierdzana i nie ma wycofania. |
| Usunięcie zatwierdzonego wydarzenia | Istniejące wydarzenie `Approved` | `EmConflictException`; wydarzenie nie jest oznaczone jako usunięte, obraz nie jest usuwany, transakcja zostaje wycofana. |
| Brak wydarzenia | ID nieobecne w repozytorium | `EmNotFoundException`; obraz nie jest usuwany, transakcja zostaje wycofana. |

**Jak działa:** repozytorium, jednostka pracy i Blob Storage są atrapami. Testy sprawdzają miękkie usunięcie (`IsDeleted`) oraz kolejność skutków ubocznych i decyzję o zatwierdzeniu albo wycofaniu transakcji.

### Integracyjne usuwanie wydarzenia — `DeleteEventCommandHandlerIntegrationTests`

**Dane:** w bazie InMemory zapisane wydarzenie `Draft` organizatora, z obrazem `https://storage.example.com/events/poster.jpg`.

**Oczekiwany rezultat:** po usunięciu rekord nadal istnieje, ale ma `IsDeleted == true`; symulowana usługa Blob Storage otrzymuje dokładnie jedno żądanie usunięcia `poster.jpg` z kontenera `events`.

**Jak działa:** prawdziwy kontekst EF Core InMemory przechowuje organizatora i wydarzenie. Wywoływany jest serwis zarządzania wydarzeniami, a potem rekord jest ponownie pobierany bez śledzenia zmian. Weryfikuje to zapis miękkiego usunięcia przez EF oraz wywołanie zewnętrznej usługi, ale samo usunięcie pliku w Azure jest symulowane.

## Jak czytać testy

Każdy test ma zwykle trzy kroki: **przygotowanie** danych i zachowania zależności, **wywołanie** handlera lub serwisu oraz **asercje** na wynik, zmieniony obiekt, wyjątek albo liczbę wywołań. Test jednostkowy izoluje badaną logikę atrapami. Integracyjny łączy kilka prawdziwych elementów aplikacji z lekką bazą InMemory. Dzięki temu oba rodzaje testów uzupełniają się: jednostkowe precyzyjnie sprawdzają reguły, a integracyjne sprawdzają zapis i współpracę komponentów.
