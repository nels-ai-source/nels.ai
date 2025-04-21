/**
 * @see https://umijs.org/docs/max/access#access
 * */
export default function access(initialState: { currentUser?: API.CurrentUser } | undefined) {
  const { currentUser } = initialState ?? {};
  const permissions = currentUser?.permissions || [];
  const checkPermission = (permissionName: string) => {
    return permissions.includes(permissionName);
  };
  return {
    canAdmin: currentUser && currentUser.roles?.includes('admin'),
    checkAccess: (permissionName: string) => checkPermission(permissionName),
  };
}
