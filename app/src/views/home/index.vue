<template>
    <el-main class="nopadding" style="height: 100%;">
        <el-container>
            <el-aside width="300px">

            </el-aside>
            <el-container>
                <el-main>
                    <div class="page-content" style=" padding: 10px">

                    </div>
                </el-main>
                <el-footer style="height: auto; border-top: none;">
                    <Button type="primary" @click="handleSendMessage(text)">Send</Button>            
                </el-footer>
            </el-container>
        </el-container>
    </el-main>
</template>

<script>
import  Bubble from 'ant-design-x-vue';

export default {
    name: 'dashboard',
    components: {
        Bubble
    },
    data() {
        return {
            text: 'Hello World',
            messages: [
                {
                    content: 'Hello, Ant Design X Vue!',
                    role: 'user',
                },
            ],
        };
    },
};
</script>

<style>
</style>
