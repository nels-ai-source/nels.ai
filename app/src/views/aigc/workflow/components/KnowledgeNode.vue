<script setup>
import { Handle } from '@vue-flow/core';

// props were passed from the slot using `v-bind="customNodeProps"`
// eslint-disable-next-line no-unused-vars
const props = defineProps({
    id: {
        type: String,
        required: true,
    },
    data: {
        type: Object,
        required: true,
    },
    targetPosition: {
        type: String,
        required: true,
    },
    sourcePosition: {
        type: String,
        required: true,
    },
});
</script>

<template class="node-default">
    <Handle type="target" :position="targetPosition" />
    <el-text>
        <el-icon><sc-icon-home /></el-icon>
        知识库
    </el-text>
    <div>{{ data.label }}</div>
    <Handle type="source" :position="sourcePosition" />
</template>
<home />