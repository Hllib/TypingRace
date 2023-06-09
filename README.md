# Typing Race

### [About ](#about)
### [Details ](#details)
### [Demo](#demonstration)
### [Acknowledgements ](#acknowledgements)
### [Feedback ](#feedback)

## About
This game is a typing simulator that not only allows you to improve your blind typing speed skill, but also brings a game element of racing into training. The faster and the more precise you type - the faster your car moves. 
You can choose the difficulty level, which affects the speed of a rival car, as well as the complexity of the words you need to type. If a rival car has overtook you, you have 10 seconds to recover your position.

You can track your statistics in the game - your actual 'words per minute' typing speed, as well as the accuracy of your typing and the time you spend in the game. The speed of your car is linked to your 'adjusted words per minute' typing speed, which means not only the amount of words you type affects the speed, but the accuracy also has an effect on it.

## Details

Typing race presents a kind of an endless runner game:
- road tiles are generated dynamically, randomly shuffled, thus creating a unique surrounding area each time. 
- you can keep playing session endlessly as long as you are ahead of the rival car.
- you have some time to recover position in case the rival car overtakes you.
- you can monitor the statistics concerning your typing skills. It is saved in three categories: current stats, best ever and total/average. 

To achieve desired performance, there was created an object pooler for road tiles. The initial set of road tiles variants is generated beforehand, and the next piece is chosen randomly from the list and activated at the desired position. It gets then deactivated after the player leaves it. 


FMOD Studio was used to implement in-game audio.

## Demonstration

https://user-images.githubusercontent.com/108517092/230789145-0ad6006d-893e-449d-bb33-95141165a53d.mp4

https://user-images.githubusercontent.com/108517092/230789141-869e1961-1f97-4254-82df-b45e20236fd7.mp4

## Acknowledgements
 - [Icons8](https://icons8.com/)
 - [Pixabay](https://pixabay.com/)
 - Music by <a href="https://pixabay.com/users/teodholina-32752748/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=141320">teodholina</a> from <a href="https://pixabay.com/music//?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=141320">Pixabay</a>
## License
[MIT License](https://choosealicense.com/licenses/mit/)

## Feedback

If you have any feedback, please reach out to me via hlib.monastyrov@gmail.com
