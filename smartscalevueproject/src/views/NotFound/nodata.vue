<template>
    <div>
        <h1>未发现编号为{{ $route.params.faceNo }}的数据。</h1>
        <img alt="404 error" src="../../assets/nodata.jpg">
    </div>
</template>

<script lang="ts">import { defineComponent } from 'vue';
    export default defineComponent({
        name: 'NoData',
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
