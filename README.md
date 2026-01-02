# Event Marketplace - backend

Backend systemu Event Marketplace - platforma do organizowania oraz uczestniczenia w wydarzeniach. Odpowiada za logikę biznesową, zarządzanie rolami, autoryzację i uwierzytelnianie użytkownika oraz komunikację z bazą danych.

---

## Project Status

Projekt w fazie ciągłego rozwoju.
Podstawowe funkcjonalności dla roli Organizatora zostały już zaimplementowane. Kolejne moduły są stopniowo dodawane. Trwają pracę nad panelem Administratora.

---

## Features

- Autoryzacja i uwierzytelnianie (JWT + Refresh Token)
- Role użytkowników: Admin, Organizer, Member
- CRUD wydarzeń (Organizer)
- System statusów wydarzeń
- CI/CD + Docker
- Upload zdjęć wydarzeń do Azure Blob Storage
- Dapper do read modeli (statystyki, krótkie powiadomienia / alerty na dashboardzie administratora)

---

## Architecture

Clean Architecture
Podział na warstwy:
- API
- Application (CQRS + MediatR)
- Domain
- Infrastructure

Rozdzielenie komend i zapytań (CQRS).
Projekt został zaprojektowany zgodnie z zasadami Clean Architecture, 
co pozwala na wyraźne oddzielenie logiki biznesowej od szczegółów technicznych.

- Warstwa **Domain** nie posiada zależności od innych warstw i zawiera wyłącznie logikę biznesową.
- Warstwa **Application** implementuje przypadki użycia oraz wzorce CQRS i MediatR.
- Warstwa **Infrastructure** zawiera implementacje dostępu do danych, integracje zewnętrzne oraz szczegóły techniczne.
- Warstwa **API** pełni rolę punktu wejścia do systemu i odpowiada za obsługę żądań HTTP oraz autoryzację.

Zależności pomiędzy warstwami są skierowane do wewnątrz, co ułatwia testowanie, rozwój i utrzymanie aplikacji.

---

## Tech Stack

- .Net 9 Web API
- CQRS + MediatR
- Entity Framework Core
- PostgreSQL
- JWT + Refresh Token
- Docker
- Github Actions (CI/CD, ghcr)

---

## Authentication & Authorization

- JWT access token
- Refresh Token z możliwością unieważnienia
- Role-based authorization
- Ochrona endpointów per rola

---

## Design Decisions

- EF Core został użyty zamiast Dappera w MVP w celu zachowania spójności transakcji i prostoty implementacji.
- CQRS wprowadzony dla lepszego rozdzielenia logiki zapisu i odczytu.
- Brak asynchronicznej komunikacji w obecnej wersji – architektura przygotowana pod jej późniejsze wdrożenie.

---

## Future improvements

- SignalR dla dwukierunkowej komunikacji w czasie rzeczywistym (komentarze do eventu Admin <-> Organizer)
- Możliwość wyświetlania odległości wydarzenia od miejsca pobytu uczestnika (zewnętrzne api, generowanie Lat i Lon)
- Asynchroniczna komunikacja (RabbitMQ / Kafka)
- Background jobs dla maili
