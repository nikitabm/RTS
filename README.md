# RTS
For testing purposes (rn not  testable completely) open scene "testScene"
After hosting server and creating client player controller gets enabled and it is possible to select unit and create move command.
click on unit with left mouse button and then click somewhere on the plane.

how code works step by step and what is happening where:
1) Network manager creates server,client and Player on GameManager gameObject (no big use right now, but it will be used in future.
2) Player has SelectObject script that creates commands when you select and object and place where it should go.
3)Select Object script fires an event that is sends issued command to Command Manager script. new command queued in a  Queue of commands for this particular turn;
4)When all players are connected to the game, server sends message to connected players that the game is ready to start. Server fires an event that enables fixed update in lockstep manager (which is created by server itself, and used only by server). 
5)every fixed amount of time lockstep manager fires an event  to ask both players send turn data for +2 turn. After both data is received, it is send to other player. (might put approval of receiving of data from player) 
6) players should execute commands they have received, update  game and proceed to next turn. 
7) and so on...
