<template>
    <div>
        <!--
        Hello,PersonInfo.
        <h1>The faceNo is {{ $route.params.faceNo }}</h1>
        {{faceNo}}-{{queryname}}<br />
        {{ruleForm.post}}<br />
        -->
        <h1>更改个人信息</h1>
        <el-form ref="ruleFormRef"
                 :model="ruleForm"
                 :rules="rules"
                 label-width="120px"
                 class="demo-ruleForm"
                 :size="formSize"
                 status-icon>
            <el-form-item label="编号" prop="name">
                <label>{{faceNo}}</label>
            </el-form-item>
            <el-form-item label="姓名" prop="name">
                <el-input v-model="ruleForm.name" />
            </el-form-item>
            <el-form-item label="年龄" prop="age">
                <el-input-number v-model="ruleForm.age" :min="0" :max="200" :value-on-clear="0" />
            </el-form-item>
            <el-form-item label="性别" prop="gender">
                <el-radio-group v-model="ruleForm.gender">
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
            //const faceNo = this.$route.params.faceNo;
            //const queryname = this.$route.query.name;
            //return { faceNo, queryname, post: null }
            //return {  post: null }
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            //this.fetchData();
        },
        methods: {
            //    fetchData(): void {
            //        this.post = null;
            //        //this.loading = true;
            //        //const result = await axios.get('https://jsonplaceholder.typicode.com/posts/1')
            //        axios
            //            //.get('https://jsonplaceholder.typicode.com/posts/1')
            //            //.get("https://localhost:44368/api/GetUpdatePersonInfoUrl?gatewayId=100")
            //            //.get("/smartScaleapi/api/GetUpdatePersonInfoUrl?gatewayId=100")
            //            .get("/smartScaleapi/api/GetPersonInfoByfaceNo/100/12")
            //            .then((response) => {
            //                //console.log(response.data.title)
            //                //response.Headers.Add("Access-Control-Allow-Origin", "*")
            //                this.post = response.data
            //            })
            //    }
        }
    });
</script>
<script lang="ts" setup>
    import { reactive, ref, onMounted } from 'vue'
    import type { FormInstance, FormRules } from 'element-plus'
    import { ElMessage } from 'element-plus'
    import { useRoute, useRouter } from 'vue-router'

    const route = useRoute()
    const router = useRouter()

    const faceNo = route.params.faceNo;
    const queryname = route.query.name;
    console.log(queryname)

    //var post = null
    //const num = ref(0)
    const formSize = ref('default')
    const ruleFormRef = ref<FormInstance>()
    const ruleForm = reactive({
        name: 'Hello',
        age: '0',
        gender: 'Unknown',
        gatewayId: 'vuepage',
        post: {}
    })

    onMounted(() => {
        fetchData()
    })

    const rules = reactive<FormRules>({
        name: [
            { required: true, message: '请输入名字', trigger: 'blur' },
            { min: 0, max: 200, message: '输入范围从0到200。', trigger: 'blur' },
        ],
        gender: [
            {
                required: true,
                message: '请选择性别。',
                trigger: 'change',
            },
        ],
    })

    const submitForm = async (formEl: FormInstance | undefined) => {
        if (!formEl) return
        await formEl.validate(async (valid, fields) => {
            if (valid) {
                var jsonObject: any = ruleForm.post
                jsonObject.name = ruleForm.name
                jsonObject.age = ruleForm.age
                jsonObject.gender = ruleForm.gender
                const res = await axios.post("/smartScaleapi/api/UpdatePersonInfo/", ruleForm.post)
                ruleForm.post = res.data
                //console.log(res.data.json);
                ElMessage.success('编号"' + faceNo + '"的数据保存成功.')
                console.log('submit!')
            } else {
                ElMessage.error('编号"' + faceNo + '"的数据保存失败.')
                console.log('error submit!', fields)
            }
        })
    }

    const resetForm = (formEl: FormInstance | undefined) => {
        if (!formEl) return
        formEl.resetFields()
    }

    const fetchData = (): void => {
        var post = null
        ruleForm.post = {}
        //this.loading = true
        //const result = await axios.get('https://jsonplaceholder.typicode.com/posts/1')
        axios
            //.get('https://jsonplaceholder.typicode.com/posts/1')
            //.get("https://localhost:44368/api/GetUpdatePersonInfoUrl?gatewayId=100")
            //.get("/smartScaleapi/api/GetUpdatePersonInfoUrl?gatewayId=100")
            .get("/smartScaleapi/api/GetPersonInfoByfaceNo/" + ruleForm.gatewayId + "/" + faceNo)
            .then((response) => {
                //response.Headers.Add("Access-Control-Allow-Origin", "*")
                post = response.data
                if (!post) {
                    ElMessage.error('编号"' + faceNo + '"的数据不存在.')
                    router.push({ name: 'NoData', params: { faceNo: faceNo } })
                    console.log(router)
                    return
                }
                ruleForm.name = post.name
                ruleForm.age = post.age
                ruleForm.gender = post.gender == null ? "Unknown" : post.gender
                ruleForm.post = post
                ElMessage.success('编号"' + faceNo + '"的数据读取成功.')
            })
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
