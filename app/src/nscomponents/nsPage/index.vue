<template>
    <el-container>
        <el-header v-if="regions.includes('header')">
            <!-- Header Content Here -->
            <h1>{{ title }}</h1>
        </el-header>
        <el-container>
            <el-aside :width="asideMinWidth + 'px'" :min-width="asideMinWidth + 'px'" :max-width="asideMaxWidth + 'px'" v-if="regions.includes('aside')" :class="{ 'is-sticky': asideSticky }">
                <div v-for="item in aside" :key="item.id">
                    <!-- Render Aside Items -->
                    <component :is="item._type" v-bind="item" />
                </div>
            </el-aside>
            <el-main>
                <div v-for="item in body" :key="item.id">
                    <!-- Render Body Items -->
                    <component :is="item._type" v-bind="item" />
                </div>
            </el-main>
        </el-container>
        <el-footer v-if="regions.includes('toolbar')">
            <!-- Toolbar Content Here -->
            <div v-for="item in toolbar" :key="item.id">
                <!-- Render Toolbar Items -->
                <component :is="item._type" v-bind="item" />
            </div>
        </el-footer>
    </el-container>
</template>

<script>
export default {
    components: {},
    props: {
        _type: { type: String, default: null },
        title: { type: String, default: null },
        body: { type: Array, default: () => [] },
        id: { type: String, default: null },
        asideResizor: { type: Boolean, default: false },
        pullRefresh: { type: Object, default: () => {} },
        initApi: { type: Object, default: () => {} },
        regions: { type: Array, default: () => [] },
        aside: { type: Array, default: () => [] },
        asideSticky: { type: Boolean, default: false },
        asideMinWidth: { type: Number, default: 200 },
        asideMaxWidth: { type: Number, default: 200 },
        toolbar: { type: Array, default: () => [] },
    },
    data() {
        return {};
    },
    created() {},
    watch: {},
    mounted() {},
    methods: {},
};
</script>

<style scoped>
</style>