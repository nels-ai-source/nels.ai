<template>
    <div class="chat-list__icon">
        <el-avatar :size="32"  icon="el-icon-user"></el-avatar>
    </div>
    <div class="chat-list__main">
        <div>{{localItem.title}} {{localItem.time}}</div>
        <div class="chat-list__content">
            <p>{{ localItem.content }}</p>
        </div>
    </div>
</template>
<script>
export default {
    props: {
        item: {
            type: Object,
            default: () => {},
        },
    },
    data() {
        return {
            localItem: this.item,
            avatar: 'img/avatar.jpg',
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