FROM node:lts-alpine

ARG FRONTEND_PORT=8080
ENV FRONTEND_PORT=${FRONTEND_PORT}
ARG API_BASE_HOST=
ARG API_BASE_PORT=
ENV VUE_APP_API_BASE_HOST=${API_BASE_HOST}
ENV VUE_APP_API_BASE_PORT=${API_BASE_PORT}

# устанавливаем простой HTTP-сервер для статики
RUN npm install -g http-server

# делаем каталог 'app' текущим рабочим каталогом
WORKDIR /app

# копируем оба 'package.json' и 'package-lock.json' (если есть)
COPY package*.json ./

# устанавливаем зависимости проекта
RUN npm install

# копируем файлы и каталоги проекта в текущий рабочий каталог (т.е. в каталог 'app')
COPY . .

# собираем приложение для production с минификацией
RUN npm run build

EXPOSE ${FRONTEND_PORT}
CMD ["sh", "-c", "http-server dist --port ${FRONTEND_PORT} -P http://localhost:${FRONTEND_PORT}?"]