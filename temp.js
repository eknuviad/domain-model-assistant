/*
Useful command line snippets for debugging (change as needed before running): 

Get all class _ids and names

echo "console.log(JSON.parse(process.argv.slice(2).join('')).classes.map(c=>c._id+': '+c.name))" | node - `curl --silent --location --request GET 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/' --header 'Content-Type: application/json'`
Sample output: [ 'X', 'City', 'Y' ]


Add a class

curl --location --request POST 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/class' --header 'Content-Type: application/json' --data '{"className":"Class3","dataType":false,"isInterface":false,"x":116,"y":249,}'


Delete a class by _id

curl --location --request DELETE 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/class/61' --header 'Content-Type: application/json'


Delete multiple classes by _id

for i in 57 63 65; do curl --location --request DELETE "http://localhost:8080/classdiagram/MULTIPLE_CLASSES/class/$i" --header 'Content-Type: application/json'; done


Undo most recent change in class diagram

curl --location --request PUT 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/undo' --header 'Content-Type: application/json'
*/

      // replace var script = ... with this in index.html

      // added this
      var unity = null;
      
      var script = document.createElement("script");
      script.src = loaderUrl;
      script.onload = () => {
        createUnityInstance(canvas, config, (progress) => {
          progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {
          unity = unityInstance; // added this
          loadingBar.style.display = "none";
          fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
          };
        }).catch((message) => {
          alert(message);
        });
      };
      document.body.appendChild(script);
