<template>
    <div>
        <!--<h1>Oops, it looks like the page you're looking for doesn't exist.</h1>-->
        <img alt="404 error" src="../../assets/404.png">
    </div>
</template>

<script lang="ts">import { defineComponent } from 'vue';
    export default defineComponent({
        name: 'NotFound',
        props: {
            msg: String,
        },
    });</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    h3 {
        margin: 40px 0 0;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        display: inline-block;
        margin: 0 10px;
    }

    a {
        color: #42b983;
    }
</style>
