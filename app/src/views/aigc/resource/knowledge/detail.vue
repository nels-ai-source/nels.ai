<template>
    <el-container class="page-user">
        <el-aside style="width: 240px;">
            <el-container>
                <el-header style="height: auto;display: block;">
                    {{ form.name }}
                </el-header>
                <el-main class="nopadding">
                    <el-menu class="menu" :default-active="page">
                        <el-menu-item v-for="item in menu" :key="item.component" :index="item.component" @click="openPage">
                            <el-icon v-if="item.icon">
                                <component :is="item.icon" />
                            </el-icon>
                            <template #title>
                                <span>{{item.title}}</span>
                            </template>
                        </el-menu-item>
                    </el-menu>
                </el-main>
            </el-container>
        </el-aside>
        <el-main  class="nopadding">
            <Suspense>
                <template #default>
                    <component :is="page" />
                </template>
                <template #fallback>
                    <el-skeleton :rows="3" />
                </template>
            </Suspense>
        </el-main>
    </el-container>
</template>

<script>
import document from '@/views/aigc/resource/knowledge/document';
import recall from '@/views/aigc/resource/knowledge/recall';
import setting from '@/views/aigc/resource/knowledge/setting';
export default {
    components: {
        document,
        recall,
        setting,
    },
    data() {
        return {
            page: 'document',
            form: {
                id: '',
                name: '',
                description: '',
            },
            menu: [
                {
                    icon: 'el-icon-list',
                    title: '文档',
                    component: 'document',
                },
                {
                    icon: 'el-icon-operation',
                    title: '召回测试',
                    component: 'recall',
                },
                {
                    icon: 'el-icon-setting',
                    title: '设置',
                    component: 'setting',
                },
            ],
        };
    },
    async mounted() {
        await this.get();
    },
    methods: {
        async get() {
            var res = await this.$API.aigc.knowledge.detail.post({
                id:  this.$route.query.id,
            });
            this.form = res;
        },
        openPage(item) {
            this.page = item.index;
        },
    },
};
</script>

<style scoped>
</style>
