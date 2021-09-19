The following steps can be used to host the WebGL content via Node JS server.<br>
This is an alternative to serving the built Unity WebGL files over an Apache server whose instructions can be found here: [Apache Server Setup](https://github.com/eknuviad/domain-model-assistant/blob/main/documentation/Unity-WebGL-Apache-Server-Setup.md)<br>
NB: *This was tested on Windows 10. Steps may vary for MAC OS* <br>
## Step 1
Ensure you have Node JS installed locally. <br>
The LTS (Long Term Support) version can be installed here : (https://nodejs.org/en/)
## Step 2
Build the Unity project in WebGL and take note of the diretory in which the built files are stored.
## Step 3
Navigate to the location of the built files via the terminal. This should be the directory where the `index.html` file is located.
## Step 4
Type the command `npx http-server -o` in the terminal.<br>
This should open a browser window after starting the local server.
