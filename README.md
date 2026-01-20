# Aplikacja wspierająca rozwój dziecka

Aplikacja webowa stworzona w technologii **ASP.NET Core MVC**, której celem jest
wspieranie procesu nauki i rozwoju dziecka poprzez quizy edukacyjne,
monitorowanie postępów oraz zarządzanie umiejętnościami i aktywnościami.

Projekt wykorzystuje mechanizm **ASP.NET Core Identity** do zarządzania
użytkownikami oraz rolami.

---

## 📌 Funkcjonalności

- quizy edukacyjne (matematyka, język angielski),
- zapisywanie i przegląd wyników quizów,
- zarządzanie dziećmi, umiejętnościami i aktywnościami,
- dziennik aktywności dziecka,
- system ról i uprawnień,
- edycja profilu użytkownika.

---

## 👥 Role użytkowników

System obsługuje cztery role:

| Rola | Opis |
|-----|------|
| **Dziecko** | Rozwiązywanie quizów, podgląd własnych wyników |
| **Rodzic** | Podgląd wyników dzieci, przegląd umiejętności i aktywności |
| **Nauczyciel** | Funkcje rodzica + zarządzanie rolami użytkowników |
| **Administrator** | Pełny dostęp do systemu i danych |

Zakres dostępnych funkcji zależy od przypisanej roli.

---

## 🛠️ Technologie

- **ASP.NET Core MVC**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **SQL Server**
- **Razor Pages**
- **Doxygen** (dokumentacja techniczna)
- **LaTeX / PDF** (eksport dokumentacji)

---

## 🚀 Uruchomienie projektu

### Wymagania
- .NET SDK (zgodny z wersją projektu)
- SQL Server
- Visual Studio / VS Code
- (opcjonalnie) MiKTeX lub TeX Live – do generowania PDF z Doxygena

### Kroki
1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/twoj-login/nazwa-repozytorium.git
