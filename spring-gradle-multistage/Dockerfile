#
# docker build -t spring-gradle-multistage:0.0.1 .
# docker run -it --rm -p 8080:8080 spring-gradle-multistage:0.0.1
#

# 1. fázis: kompilálás s distributable előkészítése
FROM gradle:8.10.0-jdk21-jammy AS build-env
WORKDIR /src/
COPY . ./
RUN gradle --no-daemon bootJar

# 2. fázis: distributable átmásolása a Gradle konténerből egy egyszerű JRE-sre
FROM eclipse-temurin:21-jre-alpine
WORKDIR /usr/app
COPY --from=build-env /src/build/libs/spring-gradle-multistage.jar ./
CMD java -jar spring-gradle-multistage.jar
