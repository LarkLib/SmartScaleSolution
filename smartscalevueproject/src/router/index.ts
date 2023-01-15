import { createWebHistory, createRouter } from "vue-router";
//import Home from "@/views/Home.vue";
//import About from "@/views/About.vue";
import PersonInfo from "@/views/PersonInfo/index.vue"
import NotFound from "@/views/NotFound/index.vue"
import HelloWorld from "@/components/HelloWorld.vue"
import PersonInfoForm from "@/views/PersonInfo/form.vue"
const routes = [
    {
        path: "/",
        name: "Home",
        component: HelloWorld
    },
    {
        path: "/personinfoform",
        name: "PersonInfoForm",
        component: PersonInfoForm
    },
    {
        path: "/personinfo/:faceNo",
        name: "PersonInfo",
        component: PersonInfo,
        props: true,
    },
    {
        path: "/:catchAll(.*)",
        name: "NotFound",
        component: NotFound,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;