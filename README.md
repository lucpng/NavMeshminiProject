# NavMeshminiProject

## Features Implementées

![Screenshot](./imgReadMe.png)

* **Visitors** :
  * Les visiteurs vont d'attraction en attraction et attendant leur tour en files d'attente cohérentes.
  * " ne pointent jamais vers un autre visiteurs détruits.
  
* **Attraction** :
  * Les attractions ont plusieurs paramètres : **Capacity** (int [1; +inf]), **Duration** (int) --> Afin de pouvoir l'utiliser comme paramètre de Coroutine, **Entrance / Entry** qui sont des EmptyObject enfant du Prefab l'attraction (Une sphère verte représente l'entrée et une sphère rouge représente la sortie)
  * Les attractions gèrent seulement leurs infos personnelles et leur PoiQueue.

* **PoiQueue** :
  * Gère 2 queues distinctes : comingVisitors et visitorsInQueue afin de permettre l'implementation d'un "Pseudo" Design Pattern Observer, avec une liste d'abonnés (comingVisitor) notifiés lorsqu'un changement dans la file d'attente de l'attraction visée est survenu.

* **Wanderers** :
  * Les Wanderers atteignent une destination et en recalcul une nouvelle une fois la première atteinte. (J'ai détecté un comportement étonnant et une perte de performance assez notable récemment, mais j'ai préféré me concentrer sur les objectifs "Mandatory", je vous laisse voir par vous même).
  
* **UI** :
  * Il est possible d'ajouter et supprimer des Visiteurs et de Wanderers sans risquer de fuite mémoire ou de pointer vers un emplacement mémoire vide.
  * Le nombre courent de visiteurs et de wanderers est affiché et mis à jour à chaque modification.

* **Camera Controller** :
  * Un controller très basique de camera orthographic a été implémenté, permettant de se déplacer de haut en bas, de gauche à droite (en maintenant le clic gauche de la sourie enfoncé) et de zoomer à l'aide de la molette et de reset.
  * Reset de la position : Clic Droit
  
* **Pont / Obstacle** :
  * Un pont passant au-dessus d'un rivière et présent, dont les barrières ont été exclues du navMesh.

## Notes personnelles

**Performance** : (Sur ma relativement puissante machine) +60 FPS jusqu'à 2000 Visitors. (Valeur bien inférieure avec des Wanderers pour des raisons d'implémentation non optimisée comme expliqué plus tôt).

**Disclaimer** :
Je préfère en parler dans ce ReadMe comme "disclaimer" mais j'ai une parfaite conscience d'avoir un code parfois redondant et sans aucun doute très peu qualitatif ni efficace, je pensais avoir le temps de réaliser un refactoring quasi complet avant le rendu final mais des évènements personnels récents m'en ont empéchés. Pas d'excuse ici, seulement un petit message de prévention.
