<template>
  <div>
    <Welcome
      icon="https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*s5sNRo5LjfQAAAAAAAAAAAAADgCCAQ/fmt.webp"
      title="Hello, I'm Ant Design X"
      description="Base on Ant Design, AGI product interface solution, create a better intelligent vision~"
    />
    <Bubble.List :items="messages" />
    <Sender />
  </div>
</template>

<script lang="ts" setup name="Analysis">
import { ref, computed } from 'vue'
import { Welcome, Bubble, Sender } from 'ant-design-x-vue'

const messages = [
  {
    content: 'Hello, Ant Design X Vue!',
    role: 'user'
  },
  {
    content: 'Hello, Ant Design X Vue!',
    role: 'user'
  }
]
</script>

<style lang="less" scoped>
@import '@/style/index.less';
</style>
