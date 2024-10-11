# 使用 Node.js 作為基礎映像
FROM node:20 AS build

# 設定工作目錄
WORKDIR /app

# 複製 package.json 和 package-lock.json
COPY package*.json ./

# 安裝依賴
RUN yarn install

# 複製應用程式碼
COPY . .

# 複製 SSL 證書到容器
COPY certs/landmarklinux.client.pem /etc/ssl/certs/cert.pem
COPY certs/landmarklinux.client.key /etc/ssl/private/key.pem


# 暴露開發伺服器使用的端口
EXPOSE 5174

# 啟動 Vue 開發伺服器
CMD ["yarn", "dev"]
