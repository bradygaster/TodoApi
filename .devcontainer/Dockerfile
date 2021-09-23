# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.194.0/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 5.0, 3.1, 2.1
ARG VARIANT="5.0"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

# [Choice] Node.js version: none, lts/*, 16, 14, 12, 10
ARG NODE_VERSION="none"
RUN if [ "${NODE_VERSION}" != "none" ]; then su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

# [Option] Install Azure CLI
ARG INSTALL_AZURE_CLI="false"
COPY library-scripts/*.sh library-scripts/*.env /tmp/library-scripts/
RUN if [ "$INSTALL_AZURE_CLI" = "true" ]; then bash /tmp/library-scripts/azcli-debian.sh; fi \
    && apt-get clean -y && rm -rf /var/lib/apt/lists/* /tmp/library-scripts

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install global node packages.
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g <your-package-here>" 2>&1

RUN wget https://download.visualstudio.microsoft.com/download/pr/ddb6b148-6882-46dd-9885-a803b460a66d/7e773236a05890fd2799045b96a85526/dotnet-sdk-6.0.100-rc.1.21463.6-linux-musl-x64.tar.gz
RUN mkdir -p $HOME/dotnet && tar zxf dotnet-sdk-6.0.100-rc.1.21463.6-linux-musl-x64.tar.gz -C $HOME/dotnet
RUN export DOTNET_ROOT=$HOME/dotnet
RUN export PATH=$PATH:$HOME/dotnet