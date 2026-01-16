# BoardGameTools

BoardGameTools est une application destinée à accompagner une **ludothèque**
et à proposer des **outils d’aide autour des jeux de société**.

L’objectif du projet est de centraliser des informations utiles liées aux jeux
et de fournir des fonctionnalités facilitant leur utilisation et leur compréhension
pendant les parties.

## Objectif de l’application

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

## Stack technique

- **.NET / C#**
- **ASP.NET Core** (API)
- **Entity Framework Core**
- **Architecture en couches**
- **Tests unitaires** (xUnit)