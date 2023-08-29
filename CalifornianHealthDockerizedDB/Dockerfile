# Choose exact tag (not 'latest'), to be sure that new version will not break creating image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables, not to have to write them with docker run command
ENV SA_PASSWORD Password+123
ENV ACCEPT_EULA Y
ENV MSSQL_PID Developer

# Set the working directory
WORKDIR /app

# Copy initialization scripts and database files
# Remove CalifornianHealthDockerizedDB for docker-compose without others services
COPY CalifornianHealthDockerizedDB/entrypoint.sh ./
COPY CalifornianHealthDockerizedDB/init-db.sh ./
COPY CalifornianHealthDockerizedDB/CH.mdf ./
COPY CalifornianHealthDockerizedDB/CH_log.ldf ./
COPY CalifornianHealthDockerizedDB/CH_tests.mdf ./
COPY CalifornianHealthDockerizedDB/CH_tests_log.ldf ./

USER root 
RUN chmod +x entrypoint.sh init-db.sh

# Run Microsoft SQL Server and initialization script (at the same time)
CMD /bin/bash ./entrypoint.sh
