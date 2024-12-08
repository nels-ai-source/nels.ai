<template>
    <div class="chat-list__icon">
        <el-avatar :size="32" icon="el-icon-magic-stick"></el-avatar>
    </div>
    <div class="chat-list__main">
        <div>{{localItem.title}} {{localItem.time}}</div>
        <sc-md-code-editor v-model="localItem.content" height="100%" width="100%" mode="preview"></sc-md-code-editor>
        
    </div>
</template>
<script>
import { defineAsyncComponent } from 'vue';
const scMdCodeEditor = defineAsyncComponent(() =>
    import('@/components/scMdCodeEditor')
);
export default {
    components: {
        scMdCodeEditor,
    },
    props: {
        item: {
            type: Object,
            default: () => {},
        },
    },
    data() {
        return {
            localItem: this.item,
            avatar: 'img/logo.png',
        };
    },
};
</script>
<style scoped>
.chat-list__icon {
    width: 32px;
    margin-right: 15px;
}
.chat-list__main {
    flex: 1;
}
.chat-list__content {
    background-color: rgba(148, 152, 247, 0.44);
    border-radius: 8px;
    padding: 12px 16px;
}
</style>