const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
    transpileDependencies: true
})
////////////////////////////////////////////////////////////////

module.exports = {
    devServer: {
        //https: true,
        proxy: {
            "/smartScaleapi": {
                target: "http://localhost:1333",
                //target: "http://localhost:44368",
                ws: false,
                changeOrigin: true,
                secure: false,
                //pathRewrite:{'^/api': '/api'}
                pathRewrite: { '/smartScaleapi': '/' }
            }
        }
    }
};