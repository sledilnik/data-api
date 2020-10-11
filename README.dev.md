# Dev notes

## Development with VS Code

You can use [Remote Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) extension to develop this project on any platform. All required tools will be insalled in docker container and VS Code will operate from there without touching your system.

You can use VS Code integrated terminal (Ctrl+\`) to use CLI inside docker (eg. using `dotnet command`). You can lunch debugger (F5) and debug project or run int in hot-reload mode (Ctrl+Shift+P > Run task > watch).

To start developing inside container, install extension and use command `Remote-Containers: Reopen in container`