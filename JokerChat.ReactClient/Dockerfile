FROM node:12.9.1 as build-deps
WORKDIR /usr/src/app
COPY package.json yarn.lock ./
RUN yarn
COPY . ./
ARG ENVIRONMENT=Development
COPY src/configuration.$ENVIRONMENT.json ./src/configuration.json
RUN yarn build
FROM nginx:1.17.3-alpine
COPY --from=build-deps /usr/src/app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
