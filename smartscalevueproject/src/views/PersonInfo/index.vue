<template>
    <div>
        Hello,PersonInfo.
        <h1>The faceNo is {{ $route.params.faceNo }}</h1>
        {{faceNo}}-{{queryname}}<br />
        {{post}}<br />
        <el-form ref="ruleFormRef"
                 :model="ruleForm"
                 :rules="rules"
                 label-width="120px"
                 class="demo-ruleForm"
                 :size="formSize"
                 status-icon>
            <el-form-item label="编号" prop="name">
                <label>{{post.faceNo}}</label>
            </el-form-item>
            <el-form-item label="姓名" prop="name">
                <el-input v-model="post.name" />
            </el-form-item>
            <el-form-item label="年龄" prop="age">
                <el-input-number v-model="post.age" :min="0" :max="200" :value-on-clear="0" />
            </el-form-item>
            <el-form-item label="性别" prop="resource">
                <el-radio-group v-model="post.gender">
                    <el-radio label="Man">男</el-radio>
                    <el-radio label="Female">女</el-radio>
                    <el-radio label="Unknown">未知</el-radio>
                </el-radio-group>
            </el-form-item>
            <el-form-item>
                <el-button type="primary" @click="submitForm(ruleFormRef)">
                    提交
                </el-button>
                <el-button @click="resetForm(ruleFormRef)">重置</el-button>
            </el-form-item>
        </el-form>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import axios from 'axios'

    export default defineComponent({
        name: 'PersonInfo',
        props: {
            msg: String,
            //name: String,
        },
        data() {
            const faceNo = this.$route.params.faceNo;
            const queryname = this.$route.query.name;
            return { faceNo, queryname, post: null }
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        methods: {
            fetchData(): void {
                this.post = null;
                //this.loading = true;
                //const result = await axios.get('https://jsonplaceholder.typicode.com/posts/1')
                axios
                    //.get('https://jsonplaceholder.typicode.com/posts/1')
                    //.get("https://localhost:44368/api/GetUpdatePersonInfoUrl?gatewayId=100")
                    //.get("/smartScaleapi/api/GetUpdatePersonInfoUrl?gatewayId=100")
                    .get("/smartScaleapi/api/GetPersonInfoByfaceNo/100/12")
                    .then((response) => {
                        //console.log(response.data.title)
                        //response.Headers.Add("Access-Control-Allow-Origin", "*")
                        this.post = response.data
                    })
            }
        }
    });
</script>
<script lang="ts" setup>
    import { reactive, ref } from 'vue'
    import type { FormInstance, FormRules } from 'element-plus'

    //const num = ref(0)
    const formSize = ref('default')
    const ruleFormRef = ref<FormInstance>()
    const ruleForm = reactive({
        name: 'Hello',
        resource: '',
    })

    const rules = reactive<FormRules>({
        name: [
            { required: true, message: 'Please input Activity name', trigger: 'blur' },
            { min: 0, max: 200, message: 'Length should be 3 to 5', trigger: 'blur' },
        ],
        resource: [
            {
                required: true,
                message: '请选择性别。',
                trigger: 'change',
            },
        ],
    })

    const submitForm = async (formEl: FormInstance | undefined) => {
        if (!formEl) return
        ruleForm.resource = "Man"
        await formEl.validate((valid, fields) => {
            if (valid) {
                console.log('submit!')
            } else {
                console.log('error submit!', fields)
            }
        })
    }

    const resetForm = (formEl: FormInstance | undefined) => {
        if (!formEl) return
        formEl.resetFields()
    }
</script>

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
>
