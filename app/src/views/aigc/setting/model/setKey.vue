<template>
    <el-dialog :title="$t('model.setKeyTitle')" v-model="visible" :width="600" destroy-on-close @closed="$emit('closed')">
        <el-form :model="form" ref="setKeyForm" label-width="100px">
            <el-form-item required :label="$t('model.accessKey')" prop="accessKey">
                <el-input v-model="form.accessKey" type="password" show-password clearable></el-input>
            </el-form-item>
            <el-form-item :label="$t('model.secretKey')" prop="secretKey">
                <el-input v-model="form.secretKey" type="password" show-password clearable></el-input>
            </el-form-item>
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
            mode: 'add',
            visible: false,
            isSaveing: false,
            form: {
                ids: [],
                accessKey: '',
                secretKey: '',
                properties: [],
            },
        };
    },
    mounted() {},
    methods: {
        open(mode = 'add') {
            this.mode = mode;
            this.visible = true;
            return this;
        },
        submit() {
            this.$refs.setKeyForm.validate(async (valid) => {
                if (valid) {
                    this.isSaveing = true;
                    try {
                        await this.$API.aigc.modelInstance.setKey.post(
                            this.form
                        );

                        this.$emit('success', this.form, this.mode);
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
