# Step 1: Build environment (Node.js)
FROM node:22 AS build

# Set working directory and copy necessary files
WORKDIR /app/fe/fakebook-ui
COPY ./fe/fakebook-ui/package*.json ./

# Install dependencies
RUN npm install

# Copy all application files and build the Angular app
COPY ./fe/fakebook-ui ./
RUN npm run build --prod --output-path=/app/fe/fakebook-ui/dist/fakebook-ui

# Step 2: Serve with Nginx
FROM nginx:alpine

# Copy the built Angular app to Nginx's default serving directory
COPY --from=build /app/fe/fakebook-ui/dist/fakebook-ui/browser /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx to serve the app
CMD ["nginx", "-g", "daemon off;"]

# TODO: Create config file and attach while build images