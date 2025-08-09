# 🧠 Back-end .NET API REST – Projet 7 (OpenClassrooms)

Ce projet est une API REST développée en C# avec ASP.NET Core, dans le cadre du parcours développeur .NET d’OpenClassrooms. Il illustre les principes de l’architecture **Domain-Driven Design (DDD)**, l’utilisation des **DTOs**, et l’implémentation de l’authentification via **JWT** et **Microsoft Identity**.

---

## 🧱 Architecture

Le projet est structuré selon les principes du DDD :

| Couche | Description |
|-------|-------------|
| `Findexium.Domain` | Entités métier, interfaces des repositories, logique métier |
| `Findexium.Infrastructure` | Implémentation des repositories, configuration EF Core |
| `P7CreateRestApi` | Couche API exposant les endpoints REST |
| `FindexiumApi.tests` | Tests d’intégration de l’API |
| `FindexiumDomain.tests` | Tests unitaires des règles métier |
| `FindexiumInfrastructure.tests` | Tests des accès aux données |

---

## 📦 Technologies utilisées

- ASP.NET Core (.NET 6+)
- Entity Framework Core (Code-First)
- Microsoft Identity + JWT
- xUnit pour les tests
- MSSQL Server

---

## 🧾 DTOs

Les **Data Transfer Objects (DTOs)** permettent :
- De séparer les modèles exposés par l’API des entités métier
- De faciliter la validation et le mapping
- De réduire les données transférées

Exemples :
- `UserDto`
- `ProductDto`
- `LoginDto`

---

## 🔐 Authentification & Sécurité

L’authentification est basée sur **JWT** et **Microsoft Identity** :
- Création de tokens sécurisés
- Attribution de rôles et permissions
- Middleware `UseAuthentication()` et `UseAuthorization()` dans `Program.cs`

---

## ⚙️ Configuration

Dans `appsettings.json`, configure la chaîne de connexion :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;MultipleActiveResultSets=true"
}
