<template>
    <ul class="ant-conversations css-var-r4c ">
        <li v-for="group in data" v-bind:key="group.id">
            <div class="ant-conversations-group-title">
                <span class="ant-typography css-var-r4c ">
                    <div class="ant-space ant-space-horizontal ant-space-align-center ant-space-gap-row-small ant-space-gap-col-small css-var-r4c ">
                        <div class="ant-space-item"><span>{{group.lable}}</span></div>
                    </div>
                </span>
            </div>
            <ul class="ant-conversations-list">
                <li class="ant-conversations-item" v-for="item in group.items" v-bind:key="item.id">
                    <span class="ant-typography ant-typography-ellipsis ant-conversations-label css-var-r4c " :aria-label="item.lable">
                        {{item.lable}}
                    </span>
                </li>
            </ul>
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
    },
    data() {
        return {
            data: [...this.items],
        };
    },
    watch: {
        items(val) {
            this.data = val;
        },
    },
};
</script>
<style scoped>
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
}
.ant-conversations .ant-conversations-list .ant-conversations-label {
    color: var(--ant-color-text-description);
}
</style>