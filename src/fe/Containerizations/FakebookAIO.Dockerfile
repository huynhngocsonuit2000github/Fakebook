# Step 1: Build environment (Node.js)
FROM node:22 AS build


# Define build argument for environment (default to 'production')
ARG ENVIRONMENT=compose

# Set working directory and copy necessary files
WORKDIR /app/fe/fakebook-aio
COPY ./fe/fakebook-aio/package*.json ./

# Install dependencies
RUN npm install

# Copy all application files and build the Angular app
COPY ./fe/fakebook-aio ./
RUN echo "Building with configuration: $ENVIRONMENT"
RUN npx ng build --configuration=$ENVIRONMENT --output-path=/app/fe/fakebook-aio/dist/fakebook-aio

# Step 2: Serve with Nginx
FROM nginx:alpine

# Copy the built Angular app to Nginx's default serving directory
COPY --from=build /app/fe/fakebook-aio/dist/fakebook-aio/browser /usr/share/nginx/html

# Copy custom Nginx configuration
COPY ./fe/Containerizations/Ngix/default.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Start Nginx to serve the app
CMD ["nginx", "-g", "daemon off;"]