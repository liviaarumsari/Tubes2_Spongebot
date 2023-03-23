# Tubes2_Spongebot
## Tugas Besar IF2211 Strategi Algoritma

## **Table of Contents**
* [General Information](#general-information)
* [Requirements](#requirements)
* [How to Run and Compile (Windows)](#how-to-run-and-compile-windows)
* [Screenshot](#screenshot)
* [Program Structure](#program-structure)
* [Author](#author)

## **General Information**
>Finding treasure(s) in a maze can be solved by using search algorithm. Maze used by the program is a *n x m* maze which user can input their custom maze from a .txt file. Here are the symbol of the maze:
>| Symbol    | Description |
|------------|--------------|
| K | Start Point |
| R | Routes |
| T | Treasure |
| X | Wall |

In this repository, the solution to find the treasure(s) in mazae problem is implemented using the *TSP, BFS, and DFS* algorithm. Program will display the path of solution in GUI.

## **Requirements**
To use this program, you will need to install **.Net** (https://dotnet.microsoft.com/en-us/download) on the device you are using. You will also need **Visual Studio** (https://visualstudio.microsoft.com/) to be installed before running the program.

## **How to Run and Compile (Windows)**
### **Setup**
1. Clone this repository <br>
```sh 
$ git clone https://github.com/liviaarumsari/Tubes2_Spongebot
```
2. Open this repository in terminal
### **Compile (optional)**

### **Run**
1. Change the directory to the 'bin' folder <br>
```sh 
$ cd bin
```



## **Screenshot**

<img src="doc/home1.jpg"> 
<img src="doc/home2.jpg"> 
<img src="doc/fig.jpg"> 


## **Program Structure**
```
.
│   .gitignore
│   README.md
|
├───bin
|   └───main.exe
|
├───doc
|   └───Tubes2_K2_13521094_Spongebot.pdf
|
├───test
|   └───board1.txt
|       board2.txt
|       board3.txt
|
└───src/Spongebot
     |
     └───Algorithms
          └───BFS.cs
              DFS.cs
         Enums
          └───CellType.cs
         Exceptions
          └───Exceptions.cs
         IO
          └───FileIO.cs
         Objects
          └───Board.cs
              Cell.cs
              MazePath.cs
              Point.cs 

```

## Authors

| Name                  | GitHub                                            | NIM                  |
| --------------------- | ------------------------------------------------- | --------------------- |
| Alexander Jason       | [AJason36](https://github.com/AJason36)           | 13521100 |
| Angela Livia Arumsari | [liviaarumsari](https://github.com/liviaarumsari) | 13521094 |
| Rinaldy Adin   | [Rinaldy-Adin](https://github.com/Rinaldy-Adin)           | 13521134 |
