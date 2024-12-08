

<template>
    <v-md-editor v-model="contentValue" :height="_height" :left-toolbar="leftToolbar" :right-toolbar="rightToolbar" :mode="mode" :codemirror-config="codemirrorConfig"></v-md-editor>
</template>

<script>
export default {
    props: {
        modelValue: {
            type: String,
            default: '',
        },
        mode: {
            type: String,
            default: 'edit',
        },
        leftToolbar: {
            type: String,
            default: 'undo redo | h bold italic | ul ol hr | code',
        },
        rightToolbar: {
            type: String,
            default: 'preview fullscreen',
        },
        height: {
            type: [String, Number],
            default: 300,
        },
        codemirrorConfig: {
            type: Object,
            default: () => {
                return { lineNumbers: false };
            },
        },
    },
    data() {
        return {
            contentValue: this.modelValue,
            coder: null,
            opt: {
                theme: this.theme, //主题
                styleActiveLine: true, //高亮当前行
                lineNumbers: false, //行号
                lineWrapping: false, //自动换行
                tabSize: 4, //Tab缩进
                indentUnit: 4, //缩进单位
                indentWithTabs: true, //自动缩进
                mode: this.mode, //语言
                readOnly: this.readOnly, //只读
                ...this.options,
            },
        };
    },
    computed: {
        _height() {
            return Number(this.height)
                ? Number(this.height) + 'px'
                : this.height;
        },
    },
    watch: {
        modelValue(val) {
            this.contentValue = val;
        },
        contentValue(val) {
            this.$emit('update:modelValue', val);
        },
    },
    mounted() {},
    methods: {
        formatStrInJson(strValue) {
            return JSON.stringify(JSON.parse(strValue), null, 4);
        },
    },
};
</script>

<style scoped>
</style>
