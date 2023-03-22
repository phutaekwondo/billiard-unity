# Summary
This asset pack incuded:
+ A Set9Balls prefab (for Nine Balls Pool Game) 
+ A Set15Balls prefab (for Eight Balls Pool Game)
+ Demo Scene which simply set both sets above in a plane
+ Textures
+ Balls Shader (which help you custom the color or texture of the balls)

# Change the balls's size
The balls-set prefabs have been attached with BallsSetConfig which help you change the balls's size if you want to scale them to fit in your setted up enviroment.
Steps to set the balls's size:
1. add the balls-set prefab to your scene.
2. find the Balls Set Config in the Inspector
3. change the field "Config Ball Radius" the the number that you want
4. click "Set Balls Radius" button

note: If you don't click "Set Balls Radius" the number in "Config Ball Radius" will not be applied.

# Custom the ball's color and texture
You can custom the ball's color and decide the ball is stripe or not
Steps:
+ select a ball in the balls-set prefab
+ in the Inspector, expend the "Ball Material" component
+ expend the "Surface Inputs"
+ custom the "Color", "IsStripe", "NumberTexture" as you want