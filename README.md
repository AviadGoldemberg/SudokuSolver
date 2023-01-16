# C# Optimized Sudoku Solver

## Introduction
Sudoku is a number-placement puzzle where the goal is to fill a grid with digits so that each row, column and sub-grid contains all digits from 1 to N. The player is given a partially completed grid and must use logic to fill in the remaining blank cells according to the rules of the game. The rules are that each digit can only appear once in each row, column and sub-grid.


## Description
Sudoku solver in C# using Dancing Links and Backtracking algorithm. This Sudoku solver is optimized for memory and runtime efficiency by using special data structures.
Able to solve 1x1, 4x4, 9x9, 16x16, and 25x25 Sudoku boards.

## Features
* High-performance Dancing Links implementation.
* Special Sparse Matrix data structure implementation to be optimized to the use of the Dancing Links algorithm.
* Support for various board sizes (1x1, 4x4, 9x9, 16x16, 25x25)

## Requirements
* .Net 4.7.2 or higher.

## How it works
### Dancing Links Algorithm
The Dancing Links algorithm is an efficient technique for solving exact cover problems, such as Sudoku. The program create a sparse matrix which represent the Sudoku board as exact cover problem and the Dancing Links algorithm solve it. The Dancing Links algorithm is efficient because it uses 'Cover' and 'Uncover' operations to control the search space. 'Covering' means removing some rows from the search so the algorithm can focus on other possibilities, and 'Uncovering' means adding them back in. This helps the algorithm to quickly find the correct solution by only looking at the most promising options and avoiding useless ones. [This](https://www.kth.se/social/files/58861771f276547fe1dbf8d1/HLaestanderMHarrysson_dkand14.pdf) paper explain Dancing Links algorithm to solve Sudoku as exact cover problem. I used it to develop this algorithm.
### Backtracking
Backtracking is a method for solving Sudoku by filling in squares with numbers that meet the constraints of the game, and undoing choices that lead to a dead-end. It starts by filling cell and if it can't find a valid number, it backtracks to the previous square and tries a different number.
### Optimized algorithm
There is an option to choose optimized algorithm depending on the board. The algorithm choose to use the Dancing Links algorithm or Backtracking. In most cases, the Backtracking will be faster than Dancing Links when there is little empty cells in the Sudoku board. So, the algorithm check if there is little empty cell at the board. If there is little empty cells the algorithm to solve the board will be Backtracking. Otherwise, the algorithm will be Dancing Links.


## Usage
The sln is in SudokuSolver folder.
To solve Sudoku board, enter the string which represent the sudoku board and the solved board will be displayed.
```
Enter board string: 001300002079000000020670903000967300750001049080503100040702530205806700107405060
Enter 1 to solve with dancing links.
Enter 2 to solve with Backtracking.
Enter 3 to solve with optimized algorithm.
Enter your choice: 3
Solving with Dancing Links...
Solved in [0ms]
861359472379124856524678913412967385753281649986543127648792531235816794197435268
```
You can choose solving using Dancing Links, Backtracking, or optimized algorithm. The optimized algorithm will choose if to solve using Dancing Links or Backtracking.
It is highly recommended to run the project as Realese mode in order to get the best result.

## Potential Feature Upgrades

### Constraint Propagation
Constraint propagation is a technique used to solve sudoku puzzles. It works by using information from known numbers to eliminate possibilities for unknown numbers. This process helps to narrow down the options for each cell, making the solution more efficient. It's a way to use logical reasoning to solve the puzzle instead of trying every possibility.

#### Optimizing Dancing Links Sudoku with Constraints
Incorporating constraints into a Dancing Links Sudoku solution can greatly optimize its performance. These constraints, such as the placement of certain numbers, provide the algorithm with additional information about the problem, allowing it to eliminate possibilities and make more informed decisions. The result is a more efficient and accurate solution, achieved by reducing the search space.