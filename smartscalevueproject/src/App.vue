<template>
    <!--<img alt="Vue logo" src="./assets/logo.png">-->
    <!--<HelloWorld msg="Welcome to Your Vue.js + TypeScript App" />-->
    <!--<div id="nav">
        <router-link to="/"> Home </router-link> |
        <router-link to="/personinfo"> PersonInfo </router-link>
    </div>-->
    <router-view />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
//import HelloWorld from './components/HelloWorld.vue';

export default defineComponent({
  name: 'App',
  components: {
    //HelloWorld
  }
});
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
