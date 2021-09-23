# Guide to WebCORE

## Official resources

Project repository: https://bitbucket.org/mcgillram/touchcore-web/src/master/

Setup guide: See relevant links in [TouchCORE Wiki](https://bitbucket.org/mcgillram/touchram/wiki/Home).

## Running WebCORE

Modify the `run_webcore.sh` script according to your local installation. The final command **must** be on one line.
Before running for the first time, you must build the project
using Eclipse or Maven.

## Useful REST API commands

The following `curl` commands are useful for debugging and can
be translated into C# code to be used within the application.


Get the entire class diagram
```bash
curl --location --request GET 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES' --header 'Content-Type: application/json'
```

Get all class `_id`s and names
```bash
echo "console.log(JSON.parse(process.argv.slice(2).join('')).classes.map(c=>c._id+': '+c.name))" | node - `curl --silent --location --request GET 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/' --header 'Content-Type: application/json'`
```
Sample output: `[ 'X', 'City', 'Y' ]`


Add a class

```bash
curl --location --request POST 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/class' --header 'Content-Type: application/json' --data '{"className":"Class3","dataType":false,"isInterface":false,"x":116,"y":249}'
```

Delete a class by `_id`

```bash
curl --location --request DELETE 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/class/61' --header 'Content-Type: application/json'
```

Delete multiple classes by `_id`

```bash
for i in 3 5 12; do curl --location --request DELETE "http://localhost:8080/classdiagram/MULTIPLE_CLASSES/class/$i" --header 'Content-Type: application/json'; done
```

Undo most recent change in class diagram

```bash
curl --location --request PUT 'http://localhost:8080/classdiagram/MULTIPLE_CLASSES/undo' --header 'Content-Type: application/json'
```

See the WebCORE documentation for more commands.
