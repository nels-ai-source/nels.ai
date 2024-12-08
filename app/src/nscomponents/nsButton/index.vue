<template>
    <el-button>1</el-button>
</template>

<script>
export default {
    props: {
        options: { type: Object, default: () => {} },
    },
    created() {
        console.log(this.options);
    },
};
</script>

<style scoped>
</style>