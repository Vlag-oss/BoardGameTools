# BoardGameTools

BoardGameTools est une application destinée à accompagner une **ludothèque**
et à proposer des **outils d’aide autour des jeux de société**.

L’objectif du projet est de centraliser des informations utiles liées aux jeux
et de fournir des fonctionnalités facilitant leur utilisation et leur compréhension
pendant les parties.

Le projet est conçu comme une **application web progressive (PWA)**, utilisable aussi bien sur desktop que sur mobile.

---

## 🎯 Objectif de l’application

BoardGameTools vise à proposer différents outils autour des jeux de société, notamment :

- 📚 **Gestion d’une ludothèque**
  - Ajouter, modifier et supprimer des jeux
  - Associer des informations personnalisées à chaque jeu

- 📝 **Notes et aides de jeu**
  - Ajouter des notes spécifiques à un jeu (ex : points de règle souvent oubliés)
  - Organiser ces notes par thème ou catégorie
  - À terme, pouvoir stocker l’intégralité des règles d’un jeu

- 🔍 **Recherche avancée (objectif long terme)**
  - Recherche full-text dans les règles et notes
  - Accès rapide à une information pendant une partie

- 🛠 **Outils spécifiques par jeu**
  - Outils d’aide au calcul ou à la prise de décision
  - Exemples :
    - calcul d’optimisation pour certaines mécaniques de jeu
    - aides spécifiques à des jeux complexes comme *Mage Knight*

Ces fonctionnalités évolueront progressivement afin de s’adapter aux besoins
et aux spécificités de chaque jeu.

## 🧱 Architecture générale

Le projet est structuré en **deux parties distinctes** :

### Back-end
- Architecture **CQRS**
- Organisation par **bounded context** et **use cases**
- Gestion de l’authentification, de la ludothèque et des règles métier

### Front-end
- Application **Vue.js 3** en **TypeScript**
- Progressive Web App (PWA)
- Pensée mobile-first, utilisable sur desktop et mobile

---

## ⚙️ Stack technique

### Back-end
- **.NET 10 / C#**
- **ASP.NET Core** (API REST)
- **Entity Framework Core**
- **PostgreSQL**
- **Architecture en couches**
- **CQRS**
- **Tests unitaires et d’intégration** (xUnit)

### Front-end
- **Vue.js 3**
- **TypeScript**
- **Vite**
- **Pinia**
- **Vue Router**
- **PWA (vite-plugin-pwa)**

---

## 📌 Remarques

Le projet est développé à des fins personnelles et pédagogiques

L’architecture et les choix techniques sont pensés pour être évolutifs

La PWA permet une utilisation confortable sur mobile pendant les parties