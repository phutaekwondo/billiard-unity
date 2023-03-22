# Table of contents
- [Table of contents](#table-of-contents)
- [Install Shader Graph (IMPORTANT)](#install-shader-graph-important)
- [Summary](#summary)
- [Change the balls's size](#change-the-ballss-size)
- [Custom the ball's color and texture](#custom-the-balls-color-and-texture)

# Install Shader Graph (IMPORTANT)
I know that you may not satified to install any dependencies package (if you did not install Shader Graph). But trust me, Shader Graph is not cost so much memory but it is awsome.
Steps:
1. In Unity, navigate to Window > Package Manager.
2. From the Package Manager, search or scroll to find the “Shader Graph” package within the Unity Registry.
3. Select the package and click the Install button.

Btw, if you don't install Shader Graph, you may see that all the balls in set are purple, that because Unity can't process the ball's material which used Shader Graph.

# Summary
This asset pack incuded:
+ A Set9Balls prefab (for Nine Balls Pool Game) 
+ A Set15Balls prefab (for Eight Balls Pool Game)
+ Demo Scene which simply set both sets above in a plane
+ Textures
+ Balls Shader (which help you custom the color or texture of the balls)

# Change the balls's size
The balls-set prefabs have been attached with BallsSetConfig which help you change the balls's size if you want to scale them to fit in your setted up enviroment.
Steps:
1. add the balls-set prefab to your scene.
2. find the Balls Set Config in the Inspector
3. change the field "Config Ball Radius" the the number that you want
4. click "Set Balls Radius" button

note: If you don't click "Set Balls Radius" the number in "Config Ball Radius" will not be applied.

# Custom the ball's color and texture
You can custom the ball's color and decide the ball is stripe or not
Steps:
1. select a ball in the balls-set prefab
2. in the Inspector, expend the "Ball Material" component
3. expend the "Surface Inputs"
4. custom the "Color", "IsStripe", "NumberTexture" as you want