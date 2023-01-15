const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true
})

module.exports = {
  devServer: {
    proxy: {
      "/api/": { 
        target: "https://localhost:44368",
        ws: false,
        changeOrigin: true,
        secure: false,
        //pathRewrite:{'^/api': '/api'}
      }
    }
  }
};