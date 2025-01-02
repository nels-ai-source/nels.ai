<template>
    <ul class="ant-conversations css-var-r4c ">
        <li :class="['ant-conversations-item', activeItemClass(item)]" v-for="item in data" v-bind:key="item.id">
            <span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " :aria-label="item.title">
                <el-input v-model="item.title" v-if="editId===item.id">
                    <template #append>
                        <el-button icon="el-icon-select" @click="handleRename(item)" />
                    </template>
                </el-input>
                <template v-else>
                    <div @click="handleChange(item)"> {{item.title}}</div>
                </template>
            </span>

            <el-dropdown trigger="click">
                <span role="img" aria-label="ellipsis" tabindex="-1" class="anticon anticon-ellipsis ant-dropdown-trigger ant-conversations-menu-icon ant-dropdown-open">
                    <el-icon> <el-icon-more-filled /></el-icon>
                </span>
                <template #dropdown>
                    <el-dropdown-menu>
                        <el-dropdown-item icon="el-icon-edit" @click="()=>{this.editId=item.id}">重命名</el-dropdown-item>
                        <el-dropdown-item icon="el-icon-delete" @click="handleDelete(item)">删除</el-dropdown-item>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>
        </li>
    </ul>
</template>
<script>
export default {
    name: 'conversations',
    components: {},
    props: {
        items: {
            type: Array,
            Required: true,
        },
        activeId: {
            type: String,
            default: '',
        },
    },
    data() {
        return {
            data: [...this.items],
            id: this.activeId,
            editId: '',
        };
    },
    methods: {
        activeItemClass(item) {
            return item?.id === this.id ? 'ant-conversations-item-active' : '';
        },
        handleChange(item) {
            if (this.activeId !== item.id) {
                this.$emit('activeChange', item.id);
            }
        },
        handleRename(item) {
            this.$emit('rename', item.id, item.title);
            this.editId = '';
        },
        handleDelete(item) {
            this.$emit('delete', item.id);
        },
    },
    watch: {
        items(val) {
            this.data = val;
        },
        activeId(val) {
            this.id = val;
        },
    },
};
</script>
<style>
.ant-conversations .ant-conversations-list {
    display: flex;
    gap: var(--ant-padding-xxs);
    flex-direction: column;
}

.ant-conversations .ant-conversations-list .ant-conversations-item {
    padding-inline-start: var(--ant-padding-xl);
}
.ant-conversations .ant-conversations-item {
    display: flex;
    height: var(--ant-control-height-lg);
    min-height: var(--ant-control-height-lg);
    gap: var(--ant-padding-xs);
    padding: 0 var(--ant-padding-xs);
    align-items: center;
    border-radius: var(--ant-border-radius-lg);
    cursor: pointer;
    transition: all var(--ant-motion-duration-mid) var(--ant-motion-ease-in-out);
    margin-bottom: var(--ant-padding-xs);
}
.ant-conversations .ant-conversations-label {
    flex: 1;
    color: var(--ant-color-text);
    white-space: nowrap; /* 防止文本换行 */
    overflow: hidden; /* 隐藏溢出的文本 */
    text-overflow: ellipsis; /* 在文本溢出时显示省略号 */
}
.ant-conversations .ant-conversations-item:hover {
    background-color: var(--ant-color-bg-text-hover);
}
.ant-conversations .ant-conversations-item-active {
    background-color: var(--ant-color-bg-text-hover);
}
.ant-conversations .ant-conversations-list .ant-conversations-label {
    color: var(--ant-color-text-description);
}

.ant-conversations .ant-conversations-item:hover .ant-conversations-menu-icon,
.ant-conversations .ant-conversations-item-active .ant-conversations-menu-icon {
    opacity: 1;
}
.ant-conversations .ant-conversations-item-active .ant-conversations-label,
.ant-conversations .ant-conversations-item-active .ant-conversations-menu-icon {
    color: var(--ant-color-text);
}
.ant-conversations .ant-conversations-menu-icon {
    opacity: 0;
    font-size: var(--ant-font-size-xl);
}
</style>