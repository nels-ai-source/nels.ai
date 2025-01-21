<template>
    <el-dialog :title="'#'+( this.form.index+1)" v-model="visible" :width="800" destroy-on-close @closed="$emit('closed')">
        <el-form :model="form" :rules="rules" :disabled="mode=='show'" ref="dialogForm" label-width="100px">
            <el-input v-model="form.content" clearable type="textarea" :rows="10"></el-input>
        </el-form>
        <template #footer>
            <el-button @click="visible=false">{{$t('form.cancel')}}</el-button>
            <el-button v-if="mode!='show'" type="primary" :loading="isSaveing" @click="submit()">{{$t('form.save')}}</el-button>
        </template>
    </el-dialog>
</template>

<script>
export default {
    emits: ['success', 'closed'],
    data() {
        return {
            visible: false,
            isSaveing: false,
            form: {
                id: '',
                content: '',
                index: '',
            },
            rules: {
                content: [
                    {
                        required: true,
                        message: this.$t('knowledge.namePlaceholder'),
                    },
                ],
            },
        };
    },
    mounted() {},
    methods: {
        open() {
            this.visible = true;
            return this;
        },
        submit() {
            this.$refs.dialogForm.validate(async (valid) => {
                if (valid) {
                    this.isSaveing = true;
                    try {
                        await this.$API.aigc.knowledgeDocutemt.update.post(this.form);

                        this.$emit('success', this.form);
                        this.visible = false;
                        this.$message.success(this.$t('form.success'));
                    } finally {
                        this.isSaveing = false;
                    }
                }
            });
        },
        setData(data) {
            Object.assign(this.form, data);
        },
    },
};
</script>

<style>
</style>
