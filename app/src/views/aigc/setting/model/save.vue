<template>
    <el-dialog :title="titleMap[mode]" v-model="visible" :width="600" destroy-on-close @closed="$emit('closed')">
        <el-form :model="form" :disabled="mode=='show'" ref="dialogForm" label-width="100px">
            <el-form-item :label="$t('model.name')" prop="name">
                {{ form.name }}
            </el-form-item>
            <el-form-item v-if="hasDeploymentName" :label="$t('model.deploymentName')" prop="deploymentName">
                <el-input v-model="form.description" clearable></el-input>
            </el-form-item>
            <el-form-item v-if="hasEndpoint" :label="$t('model.endpoint')" prop="endpoint">
                <el-input v-model="form.endpoint" clearable></el-input>
            </el-form-item>
            <el-form-item v-if="hasAccessKey" :label="$t('model.accessKey')" prop="accessKey">
                <el-input v-model="form.accessKey" type="password" show-password clearable></el-input>
            </el-form-item>
            <el-form-item v-if="hasSecretKey" :label="$t('model.secretKey')" prop="secretKey">
                <el-input v-model="form.secretKey" type="password" show-password clearable></el-input>
            </el-form-item>
            <el-form-item :label="$t('model.description')" prop="description">
                <el-input v-model="form.description" clearable type="textarea"></el-input>
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
            titleMap: {
                add: this.$t('model.addTitle'),
                edit: this.$t('model.editTitle'),
                show: this.$t('model.viewTitle'),
            },
            visible: false,
            isSaveing: false,
            form: {
                id: null,
                modelId: null,
                name: '',
                deploymentName: '',
                endpoint: '',
                accessKey: '',
                secretKey: '',
                description: '',
                properties: '',
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
            this.$refs.dialogForm.validate(async (valid) => {
                if (valid) {
                    this.isSaveing = true;
                    try {
                        if (this.mode == 'add') {
                            delete this.form.id;
                            await this.$API.aigc.modelInstance.create.post(
                                this.form
                            );
                        } else {
                            await this.$API.aigc.modelInstance.update.post(
                                this.form
                            );
                        }

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
    computed: {
        hasDeploymentName() {
            return this.form.properties.includes('DeploymentName');
        },
        hasEndpoint() {
            return this.form.properties.includes('Endpoint');
        },
        hasAccessKey() {
            return (
                this.form.properties.includes('AccessKey') && this.mode == 'add'
            );
        },
        hasSecretKey() {
            return (
                this.form.properties.includes('SecretKey') && this.mode == 'add'
            );
        },
    },
};
</script>

<style>
</style>
