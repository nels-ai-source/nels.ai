<template>
    <div class="ant-sender css-var-r4c">
        <div class="ant-sender-header" v-if="header">
            <div class="ant-sender-header-header">
                <div class="ant-sender-header-title">
                    <div class="ant-space ant-space-horizontal ant-space-align-center ant-space-gap-row-small ant-space-gap-col-small css-var-rft">
                        <div class="ant-space-item">
                            <el-icon> <el-icon-switch /> </el-icon>
                        </div>
                        <div class="ant-space-item">
                            <span class="ant-typography ant-typography-secondary css-var-rft">

                            </span>
                        </div>
                    </div>
                </div>
                <div class="ant-sender-header-close">
                    <el-button text icon="el-icon-close" class="ant-btn css-var-r4c ant-btn-icon-xxs" />
                </div>
            </div>
        </div>
        <div class="ant-sender-content">
            <div class="ant-sender-prefix">
                <span class="ant-badge css-var-r4c">
                    <el-button icon="el-icon-link" class="ant-btn css-var-r4c ant-btn-icon-sm" v-if="attachmentOptions" :disabled="loading" />
                    <el-button text icon="el-icon-delete" class="ant-btn css-var-r4c ant-btn-icon-sm" :disabled="loading" @click="handlerDelete"/>
                </span>
            </div>
            <el-input v-model="data" type="textarea" placeholder="Please input" :disabled="loading" :autosize="{ minRows: minRows, maxRows: maxRows }" class="ant-input-textarea ant-input-textarea-show-count ant-input-textarea-with-normal ant-input-textarea-with-border ant-input-textarea-with-placeholder"   resize="none"/>
            <div class="ant-sender-actions-list">
                <div class="ant-sender-actions-list-presets ant-flex css-var-r4c">
                    <el-button text icon="el-icon-close" class="ant-btn css-var-r4c ant-btn-icon-sm" :disabled="loading" @click="handleClearData" />
                    <el-button type="primary" icon="el-icon-top" class="ant-btn css-var-r4c ant-btn-icon-sm" circle :loading="loading" @click="handleSend" />
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name: 'sender',
    props: {
        text: {
            type: String,
            default: '',
        },
        loading: {
            type: Boolean,
            default: false,
        },
        header: {
            type: Object,
            default: () => null,
        },
        attachmentOptions: {
            type: Object,
            default: () => null,
        },
        minRows: {
            type: Number,
            default: 1,
        },
        maxRows: {
            type: Number,
            default: 5,
        },
    },
    data() {
        return {
            data: this.text,
        };
    },
    item: {
        items(val) {
            this.data = val;
        },
    },
    methods: {
        handlerDelete() {
            this.$emit('delete');
        },
        handleSend() {
            this.$emit('send', this.data);
        },
        handleClearData() {
            this.data = '';
        },
    },
    watch: {
        text(val) {
            this.data = val;
        },
    },
};
</script>
<style scoped>
.ant-sender {
    position: relative;
    width: 100%;
    box-sizing: border-box;
    box-shadow: var(--ant-box-shadow-tertiary);
    transition: background var(--ant-motion-duration-slow);
    border-radius: calc(var(--ant-border-radius) * 2);
    border-color: var(--ant-color-border);
    border-width: 1px;
    border-style: solid;
}
.ant-sender:hover {
    box-shadow: var(--ant-color-info-hover);
}
.ant-sender .ant-sender-header {
    border-bottom-width: var(--ant-line-width);
    border-bottom-style: solid;
    border-bottom-color: var(--ant-color-border);
}
.ant-sender .ant-sender-header-header {
    background: var(--ant-color-fill-alter);
    font-size: var(--ant-font-size);
    line-height: var(--ant-line-height);
    padding-block: calc(var(--ant-padding-sm) - var(--ant-line-width-bold));
    padding-inline-start: var(--ant-padding);
    padding-inline-end: var(--ant-padding-xs);
    display: flex;
}
.ant-sender-header-title {
    flex: auto;
}
.ant-sender .ant-sender-content {
    display: flex;
    gap: var(--ant-padding-xs);
    width: 100%;
    padding-block: var(--ant-padding-sm);
    padding-inline-start: var(--ant-padding);
    padding-inline-end: var(--ant-padding-sm);
    box-sizing: border-box;
    align-items: flex-end;
}
.ant-sender .ant-sender-prefix {
    flex: none;
}

.ant-sender .ant-sender-actions-list {
    flex: none;
    display: flex;
}
.ant-sender .ant-sender-actions-list-presets {
    gap: var(--ant-padding-xs);
}
.ant-sender .ant-sender-input {
    padding: 0;
    border-radius: 0;
    flex: auto;
    align-self: center;
    min-height: auto;
}
.ant-btn-icon-sm {
    padding: var(--ant-padding-xs);
    font-size: var(--ant-size-popup-arrow);
}
</style>