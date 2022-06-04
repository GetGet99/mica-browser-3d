# Mica Browser 3D (Alpha)

So,

* Do you wish you have multiple screens in your room?
* Have you ever heard of Windows Mixed Reality, but you don't have a headset like others?
* Have you ever tried to emulate mouse and keyboard controls to use with it, and the controls are awful?
* Do you feel that you wish you could use apps like what you saw on the internet?

Well, Mica Browser 3D can solve 2 out of 4 problems, except for the first point being the 'virtual screen' and the last point being that you can only use websites because this is a browser!

![Mica Browser 3D Sample](https://media.discordapp.net/attachments/713975600846733363/982683171558653952/unknown.png)

NOTE: GitHub, Stackoverflow, and Bing is only used as an example and are not affiliated with this software!

## Features

1. Navigate in 3D World with familiar controls to most FPS games using WASD, Spacebar, and Shift
2. Multitasking with 'virtual screens' of browsers
3. Having Mica in the background (on windows 11 only, obviously)
4. A lot of upcoming features planned

NOTE: Some things are unstable because this is Alpha!

## Controls

Most of the app controls are by using these HotKeys

1. Escape (ESC) - Toggle First-Person Control Mode and Web Control Mode
2. F1 - Create a new virtual window in front of you

BE CAREFUL: Attempting to refresh the page will make you lose progress on all windows, so please don't.

### First-Person Control Mode

Best for moving around!

1. WASD (W = Forward, S = Backward, A = Left, D = Right)
2. Space = Fly up
3. Shift = Fly down
4. The mouse cursor will be locked at the center of the application. You can move the mouse around to look around

NOTE: You can still use Mouse Cursor on the web screen in this mode, although sometimes it will be unstable.

NOTE 2: You cannot type in First-Person Control Mode. Please switch to Web Control Mode by pressing Escape to type anything in the browser

### Web Control Mode

This is the default mode when the app launches. All the First-Person Controls will be disabled. The mouse can be moved around freely without the 3D space moved around. You can focus, use hotkeys, and type in this mode!

### Screen buttons

![Screen buttons demonstration](https://media.discordapp.net/attachments/713975600846733363/982680755870240788/unknown.png)

NOTE: YouTube is only used as an example and is not affiliated with this software!

There will be two buttons on top of each screen. "N" will let you type the new URL in, and "X" will close the screen.

I will update the button to look better in the future.

## Technical: How does it work

This project uses WebView2 technology combined with Three.js to create a space with the IFrames in a specific position in 3D world. Unfortunately, the website can't be used directly in a regular web browser because it requires the app to send the information required.

The communicated messages are

1. Key Press
2. How much was the mouse moved if the screen is locked

Although it seems that these can be done in JavaScript alone, the complication starts when there is IFrame, making these keys unable to be appropriately handled. That is why it is instead handled on the app level and sent as a message.

NOTE: This project technically does not require Node.js. However, Node.js is used to make the production easier due to better Three.js typing.

## Future

In the future, I plan to (note: not in order of priority)

1. Make 'virtual screens' draggable
2. Make it less laggy
3. Proper User Interface
4. Fluent Design Controls
5. Minimizable screen
6. Teleport to the front of the selected screen
7. Favorites
8. Some built-in useful apps with basic features on the web (such as an offline Calculator. can be turned off)
9. Mica Discord Integration (can be turned off)
