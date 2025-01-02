<template>
    <el-dialog :title="titleMap[mode]" v-model="visible" :width="500" destroy-on-close @closed="$emit('closed')">
        <el-form :model="form" :rules="rules" :disabled="mode=='show'" ref="dialogForm" label-width="100px">
            <el-form-item :label="$t('knowledge.name')" prop="name">
                <el-input v-model="form.name" :placeholder="$t('knowledge.namePlaceholder')" clearable></el-input>
            </el-form-item>
            <el-form-item :label="$t('knowledge.description')" prop="description">
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
                add: this.$t('form.add'),
                edit: this.$t('form.edit'),
                show: this.$t('form.show'),
            },
            visible: false,
            isSaveing: false,
            form: {
                id: '',
                name: '',
                description: '',
            },
            rules: {
                name: [
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
                            await this.$API.aigc.knowledge.create.post(this.form);
                        } else {
                            await this.$API.aigc.knowledge.update.post(this.form);
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
};
</script>

<style>
</style>
