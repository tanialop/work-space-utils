# It contains a description about how execute this scripts.

### Add full path of this scripts into .bashrc

- Open the file `.bashrc` and add this current path after using the result: `$ pwd`.

Example:
```
# file .bashrc

export VH_CURRENT_SCRIPT_PATH=/home/ronald/personal/run-services
export PATH="$PATH:$VH_CURRENT_SCRIPT_PATH"

```

After that you need to refresh the console windown:

```
source ~/.bashrc

```

- Go to the projects.

```
Example:

$ cd /home/personal/mojix/enrollment-service
$ run-enrollment

```