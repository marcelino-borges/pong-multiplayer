# PONG MULTIPLAYER

Multiplayer game based in Pong.

I have used Photon library to manage network features.

Into the ball's OnPhotonSerializeView() method I'm reading the lag time and saving the supposed position where the ball would be considering the lag, then the FixedUpdate() method Lerps (to make it smooth) the ball between the actual position it's actual position and the supposed position we have predicted previously based on the lag.

I put in the level a small HUD to simulate network latency, package loss etc.

I'm using Photon tools to sync the transform views. In the case of the ball we consider the rigidbody, in the case of the paddles we consider only the objects transform component and in the case of the score we make Romote Procedure Calls from each client to increase the score into other clients.

The game is suffering a little bit with lag (a little more time developing would correct a few things), but serves it's purpose.