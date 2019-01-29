# DataMining
#### Data mining / AI course at Sofia Univeristy's Faculaty of Mathematics and Informatics

The first 5 tasks solve classic problems with *general computer science algorithms*. I implemented them using **C#**, as it was the language I was most comfortable with and I was also constraint on time. It turned out to be a great decision as the advanced *debugging* tools of Visual Studio and the features of the language (*LINQ*) really helped me develop the solutions faster.

The last 4 tasks are implementations of popular **machine learning** algorithms. I decided to use **python** and **jupyter notebook** to write the tasks. It was the first time I was using the language for something larger. I gradually learned the basics of the language and advanced to effectively using functions and classes. Again, it was a great decision to use python instead of C#, as I was able to take advantage of the well developed *machine learning ecosystem* and the *loads of useful libraries*. 

### 01. Frogs
Simple frog puzzle game, exploring all winning options using **DFS**.

### 02. Sliding blocks
Finding the solution to the classic sliding puzzle game, implementing **A*** and the ***heuristic***  - **Manhattan distance**.

### 03. N Queens
Version of the **8 Queens** puzzle - placing 8 queens on a chess board with none of them being threatened by another. Solved for N *(up to 10 000)* queens using **MinConflicts algorithm**.

### 04. Knapsack Problem
Another classic problem. Placing as many items as possible in a backpack with a total volume lower than the combined size of available items. Solved using a **genetic algorithm** - inspired by natural selection with three key functions - **Crossover, Mutation and Selection**.

 I did a lot of experiments with different mutation strategies *(always replacing a gene, 50/50 chance for a replacement or adding, replacing only when improving the chromosome...)*, doing crossover and mutation in a single step, adding immortal chromosomes and more.

### 05. Tic-Tac-Toe
Created an **AI bot** that plays *Tic-Tac-Toe* against a human player and *never looses*. The bot is implemented with the **min-max algorithm** and **alpha-beta pruning**. It finds the optimal move, even when placed in an unfair game *(human has placed more markers than allowed)*.

### 06. K-Nearest Neighbors 
Implementation of the **KNN algorithm**, including the *euclidean distance*, using the [iris dataset](https://archive.ics.uci.edu/ml/datasets/Iris/). 

### 07. Naive Bayes
Implementation of the **Naive Bayes** algorithm on the [Congressional Voting Records Data Set](http://archive.ics.uci.edu/ml/datasets/Congressional+Voting+Records) . Used **binning** on 10 bins. Each of the bins is used as a test set once while the other 9 are combined into a training set.

### 08. Decision Tree
Used the [Breast Cancer Data Set](https://archive.ics.uci.edu/ml/datasets/breast+cancer) to implemented a **Decision Tree**. Also tried the *sckit-learn* decision tree as comparison, for which I had to use **one-hot encoding**.

### 09. kMeans
Implemented the *unsupervised*  algorithm **kMeans**. Developed it on unbalanced data *(8 clusters, 3 of which owning 99% the points)*, where it showed it's weakness and failed to classify correctly. Added a *stopping rule* - when none of the points change class. Then applied it to normal data *(3 clusters with equal point representation)*.
